namespace HipodromoReservas.Tests.Entities
{
    public class ClientTests
    {
        [Theory]
        [InlineData(CategoryTypeEnum.Classic, 1, TimeSlotEnum.SLOT_2000, true)]
        [InlineData(CategoryTypeEnum.Classic, 2, TimeSlotEnum.SLOT_2000, false)]
        [InlineData(CategoryTypeEnum.Gold, 2, TimeSlotEnum.SLOT_2000, true)]
        [InlineData(CategoryTypeEnum.Gold, 4, TimeSlotEnum.SLOT_2000, false)]
        [InlineData(CategoryTypeEnum.Platinum, 3, TimeSlotEnum.SLOT_2000, true)]
        [InlineData(CategoryTypeEnum.Platinum, 5, TimeSlotEnum.SLOT_2000, false)]
        [InlineData(CategoryTypeEnum.Diamond, 300, TimeSlotEnum.SLOT_2000, true)]

        public void CouldReserve_ShouldRespectCategoryLimits(CategoryTypeEnum category, int daysAhead, TimeSlotEnum timeSlot, bool expected)
        {
            // Arrange
            var client = new Client(1, "Test", "User", category);
            var reservationDate = DateTime.Today.AddDays(daysAhead);

            // Act
            var result = client.CouldReserve(reservationDate, timeSlot);

            // Assert
            result.Should().Be(expected);
        }
    }
}