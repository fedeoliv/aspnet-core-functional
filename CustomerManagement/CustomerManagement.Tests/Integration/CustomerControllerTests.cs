﻿using System;
using System.Net;
using CustomerManagement.Api.Controllers;
using CustomerManagement.Api.Models;
using CustomerManagement.Logic.Model;
using CustomerManagement.Logic.Utils;
using CustomerManagement.Tests.Fakes;
using CustomerManagement.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CustomerManagement.Tests.Integration
{
    public class CustomerControllerTests : TestBase
    {
        [Fact]
        public void Create_creates_a_customer_if_no_validation_errors()
        {
            var model = new CreateCustomerModel
            {
                Industry = "Cars",
                Name = "Johnson and Co",
                PrimaryEmail = "mike@johnson.com",
                SecondaryEmail = "info@johnson.com"
            };

            Response response = Invoke(x => x.Create(model));

            response.ShouldBeOk();

            using (var db = new CustomerDatabase())
            {
                db.ShouldContainCustomer("Johnson and Co")
                    .WithPrimaryEmail("mike@johnson.com")
                    .WithSecondaryEmail("info@johnson.com")
                    .WithIndustry("Cars")
                    .WithEmailCampaign(EmailCampaign.LatestCarModels)
                    .WithStatus(CustomerStatus.Regular);
            }
        }

        [Fact]
        public void Create_can_create_a_customer_without_secondary_email()
        {
            var model = new CreateCustomerModel
            {
                Industry = "Cars",
                Name = "Johnson and Co",
                PrimaryEmail = "mike@johnson.com"
            };

            Response response = Invoke(x => x.Create(model));

            response.ShouldBeOk();
            using (var db = new CustomerDatabase())
            {
                db.ShouldContainCustomer("Johnson and Co")
                    .WithNoSecondaryEmail();
            }
        }

        [Fact]
        public void Update_updates_a_customer_if_no_validation_errors()
        {
            Customer customer = CreateCustomer("Cars");
            var model = new UpdateCustomerModel
            {
                Id = customer.Id,
                Industry = "Other"
            };

            Response response = Invoke(x => x.Update(model));

            response.ShouldBeOk();
            using (var db = new CustomerDatabase())
            {
                db.ShouldContainCustomer(model.Id)
                    .WithIndustry("Other")
                    .WithEmailCampaign(EmailCampaign.Generic);
            }
        }

        [Fact]
        public void DisableEmailing_disables_emailing()
        {
            Customer customer = CreateCustomer("Cars");

            Response response = Invoke(x => x.DisableEmailing(customer.Id));

            response.ShouldBeOk();
            using (var db = new CustomerDatabase())
            {
                db.ShouldContainCustomer(customer.Id)
                    .WithEmailCampaign(EmailCampaign.None);
            }
        }

        [Fact]
        public void Promote_promotes_customer()
        {
            Customer customer = CreateCustomer();
            var emailGateway = new FakeEmailGateway();

            Response response = Invoke(x => x.Promote(customer.Id), emailGateway);

            response.ShouldBeOk();
            using (var db = new CustomerDatabase())
            {
                db.ShouldContainCustomer(customer.Id)
                    .WithStatus(CustomerStatus.Preferred);

                emailGateway.ShouldContainNumberOfPromotionNotificationsSent(1);
            }
        }

        [Fact]
        public void Promote_cannot_promote_a_gold_customer()
        {
            Customer customer = CreateCustomer(status: CustomerStatus.Gold);
            var emailGateway = new FakeEmailGateway();

            Response response = Invoke(x => x.Promote(customer.Id), emailGateway);

            response.ShouldBeError("The customer has the highest status possible");
            emailGateway.ShouldContainNumberOfPromotionNotificationsSent(0);
        }

        public Customer CreateCustomer(string industryName = null, CustomerStatus? status = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                Industry industry = new IndustryRepository(unitOfWork).GetByName(industryName ?? "Cars").Value;

                var customer = new Customer((CustomerName)"Customer Name", (Email)"some@email.com", null, industry);
                if (status == CustomerStatus.Gold)
                {
                    customer.Promote();
                    customer.Promote();
                }
                new CustomerRepository(unitOfWork).Save(customer);

                unitOfWork.Commit();

                return customer;
            }
        }

        protected Response Invoke(Func<CustomerController, IActionResult> action, FakeEmailGateway emailGateway = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var controller = new CustomerController(unitOfWork, emailGateway ?? new FakeEmailGateway());
                var result = (ObjectResult)action(controller);
                
                var statusCode = (HttpStatusCode)result.StatusCode;
                var envelope = (Envelope<string>)result.Value;

                return new Response(envelope.ErrorMessage, statusCode);
            }
        }
    }
}
