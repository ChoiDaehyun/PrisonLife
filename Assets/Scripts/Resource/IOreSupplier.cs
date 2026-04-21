using System;

public interface IOreSupplier
{
    event Action<int> OnOreSupplied;
}