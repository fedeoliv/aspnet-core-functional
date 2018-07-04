﻿using CSharpFunctionalExtensions;
using CustomerManagement.Api.Controllers.Common;
using CustomerManagement.Api.Models;
using CustomerManagement.Logic.Model;
using CustomerManagement.Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CustomerManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly CustomerRepository _customerRepository;
        private readonly IEmailGateway _emailGateway;

        public CustomerController(UnitOfWork unitOfWork, IEmailGateway emailGateway)
            : base(unitOfWork)
        {
            _customerRepository = new CustomerRepository(unitOfWork);
            _emailGateway = emailGateway;
        }
        
        [HttpGet]
        public ActionResult<string> Index()
        {
            var title = "Customer Management API sample";
            return title;
        }

        [HttpGet("create")]
        public IActionResult Create(CreateCustomerModel model)
        {
            Result<CustomerName> customerName = CustomerName.Create(model.Name);
            Result<Email> primaryEmail = Email.Create(model.PrimaryEmail);
            Result<Maybe<Email>> secondaryEmail = GetSecondaryEmail(model.SecondaryEmail);
            Result<Industry> industry = Industry.Get(model.Industry);

            var result = Result.Combine(customerName, primaryEmail, secondaryEmail, industry);

            if (result.IsFailure)
            {
                return Error(result.Error);
            }

            var customer = new Customer(customerName.Value, primaryEmail.Value, secondaryEmail.Value, industry.Value);
            _customerRepository.Save(customer);

            return Ok();
        }

        private Result<Maybe<Email>> GetSecondaryEmail(string secondaryEmail)
        {
            if (secondaryEmail == null)
            {
                return Result.Ok<Maybe<Email>>(null);
            }

            return Email.Create(secondaryEmail)
                .Map(email => (Maybe<Email>)email);
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Maybe<Customer> customerOrNull = _customerRepository.GetById(id);

            if (customerOrNull.HasNoValue)
            {
                return Error($"Customer with such Id is not found: {id}");
            }

            Customer customer = customerOrNull.Value;

            var model = new
            {
                customer.Id,
                Name = customer.Name.Value,
                PrimaryEmail = customer.PrimaryEmail.Value,
                SecondaryEmail = customer.SecondaryEmail.HasValue ? customer.SecondaryEmail.Value.Value : null,
                Industry = customer.EmailingSettings.Industry.Name,
                customer.EmailingSettings.EmailCampaign,
                customer.Status
            };

            return Ok(model);
        }

        [HttpDelete("{id}/emailing")]
        public IActionResult DisableEmailing(long id)
        {
            Maybe<Customer> customerOrNull = _customerRepository.GetById(id);

            if (customerOrNull.HasNoValue)
            {
                return Error($"Customer with such Id is not found: {id}");
            }

            Customer customer = customerOrNull.Value;
            customer.DisableEmailing();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(UpdateCustomerModel model)
        {
            Result<Customer> customerResult = _customerRepository.GetById(model.Id)
                .ToResult($"Customer with such Id is not found: {model.Id}");

            Result<Industry> industryResult = Industry.Get(model.Industry);

            return Result.Combine(customerResult, industryResult)
                .OnSuccess(() => customerResult.Value.UpdateIndustry(industryResult.Value))
                .OnBoth(result => result.IsSuccess ? Ok() : Error(result.Error));
        }

        [HttpPost("{id}/promotion")]
        public IActionResult Promote(long id)
        {
            return _customerRepository.GetById(id)
                .ToResult($"Customer with such Id is not found: {id}")
                .Ensure(customer => customer.CanBePromoted(), "The customer has the highest status possible")
                .OnSuccess(customer => customer.Promote())
                .OnSuccess(customer => _emailGateway.SendPromotionNotification(customer.PrimaryEmail, customer.Status))
                .OnBoth(result => result.IsSuccess ? Ok() : Error(result.Error));
        }
    }
}
