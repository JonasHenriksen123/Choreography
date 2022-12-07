namespace Stock.ServiceAPI
{
    public interface IEventService
    {
        Task HandleEvent(Model.Event @event);
    }
}
