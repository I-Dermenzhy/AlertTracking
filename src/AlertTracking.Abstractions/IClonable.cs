namespace AlertTracking.Abstractions;

public interface IClonable<TSelf>
{
    TSelf Clone();
}
