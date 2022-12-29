using Stock.Model;

namespace Stock.ServiceAPI
{
    public interface IItemService
    {
        public Item? CreateItem(Item item);
    }
}
