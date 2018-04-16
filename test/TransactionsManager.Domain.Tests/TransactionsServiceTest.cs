using System.Threading.Tasks;
using TransactionsManager.Services;
using Xunit;

namespace TransactionsManager.Domain.Tests
{
    public class TransactionsServiceTest
    {
        private TransactionsService GetTransactionService()
        {
            return new TransactionsService(new TransactionsInMemoryRepository());
        }

        [Fact]
        public async Task MarkAsFraud_SetIsFraudEqualTrue()
        {
            // Arrange
            var transactionService = GetTransactionService();
            int transactionId = 1;
            // Act
            await transactionService.MarkAsFraud(transactionId);
            var transactionMarkedAsFraud = await transactionService.FindById(transactionId);
            // Assert
            Assert.True(transactionMarkedAsFraud.IsFraud.GetValueOrDefault(false)
                && transactionMarkedAsFraud.Id == transactionId);
        }

        [Fact]
        public async Task Paginate_Return_ResultsPaginated()
        {
            // Arrange
            var transactionService = GetTransactionService();
            int pageNumber = 1, pageSize = 10;
            // Act
            var result = await transactionService.Paginate(pageNumber, pageSize);
            // Assert
            Assert.True(result.PageCount == 2
                && result.TotalItemCount == 20
                && result.PageSize == pageSize
                && result.IsFirstPage == true);
        }

        [Fact]
        public async Task Find_ValidPredicated_ReturnCorrectResults()
        {
            // Arrange
            var transactionService = GetTransactionService();
            int pageNumber = 1, pageSize = 10;
            // Act
            var result = await transactionService.Paginate(pageNumber, pageSize);
            // Assert
            Assert.True(result.PageCount == 2
                && result.TotalItemCount == 20
                && result.PageSize == pageSize
                && result.IsFirstPage == true);
        }

        [Fact]
        public async Task Register_ValidInput_CreateNewTransaction()
        {
            // Arrange
            var transactionService = GetTransactionService();
            Transaction transaction = new Transaction()
            {
                Id = 100,
                Amount = 1000,
                NameDest = "Jhon",
                NameOrig = "Mike"
            };
            // Act
            await transactionService.Register(transaction);
            var createdTransaction = await transactionService.FindById(transaction.Id);
            // Assert
            Assert.Equal(transaction, createdTransaction);
        }
    }
}
