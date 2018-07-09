using System;
using CSharpFunctionalExtensions;
using CustomerManagement.Logic.Model;
using CustomerManagement.Logic.Utils;
using Xunit;

namespace CustomerManagement.Tests.Utils
{
    public sealed class CustomerDatabase : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerDatabase()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Customer ShouldContainCustomer(string name)
        {
            var repository = new CustomerRepository(_unitOfWork);
            Customer customer = repository.GetByName(name);

            Assert.NotNull(customer);

            return customer;
        }

        public Customer ShouldContainCustomer(long id)
        {
            var repository = new CustomerRepository(_unitOfWork);
            Maybe<Customer> customer = repository.GetById(id);

            Assert.True(customer.HasValue);

            return customer.Value;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
