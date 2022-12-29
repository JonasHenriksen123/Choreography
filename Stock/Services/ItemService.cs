using Stock.Model;
using Stock.ServiceAPI;

namespace Stock.Services
{
    public class ItemService : IItemService
    {
        private readonly DatabaseContext dbContext;

        public ItemService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Item? CreateItem(Item item)
        {
            if (item == null)
            {
                return null;
            }

            item.ItemId = Guid.NewGuid();
            
            this.dbContext.Items.Add(item);
            this.dbContext.SaveChanges();

            return item;
        }
    }
}
