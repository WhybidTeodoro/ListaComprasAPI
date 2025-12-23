using ListaComprasAPI.Entities;

namespace ListaComprasAPI.Services;

public class ShoppingListService
{
    public decimal CalcularTotalLista(ShoppingList list)
    {
        if (list.Items == null || !list.Items.Any())
            return 0;

        return list.Items.Sum(item => item.Quantity * item.UnitPrice);
    }
}
