using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.IntegrationTests
{
    public class Seeding
    {
        public static void IntializeTestDB(AppDbContext db)
        {
            db.ItemTypes.AddRange(GetItemTypes());
            db.SaveChanges();
        }

        private static List<ItemType> GetItemTypes()
        {
            return new List<ItemType>(){
                new ItemType(newInstance: true) { Id = Guid.Parse("276b1f8f-f695-46f4-9db0-78ec3f358210"), Name = "Item Type 1", Description = "Description Item Type 1" },
                new ItemType(newInstance: true) { Id = Guid.Parse("37f7653c-080b-4b1d-b1db-5b467fe29762"), Name = "Item Type 2", Description = "Description Item Type 2" },
                new ItemType(newInstance: true) { Id = Guid.Parse("23a60ea1-4471-4f1f-b0f5-a25527121647"), Name = "Item Type 3", Description = "Description Item Type 3" }
            };
        }
    }
}