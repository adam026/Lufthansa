namespace Lufthansa.Data;

public interface ICopyFrom<T> 
    where T : class
{
    T CopyFrom(T other);
}