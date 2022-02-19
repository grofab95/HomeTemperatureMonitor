namespace HTM.Infrastructure.Adapters;

public interface IPersistenceInitializer
{
    Task Initialize();
}