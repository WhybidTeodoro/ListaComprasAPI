using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListaComprasAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameShoppingListTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
       name: "ShoppingList",
       newName: "ShoppingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_ShoppingList_ShoppingListId",
                table: "ShoppingItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
        name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
        table: "ShoppingItems");

            migrationBuilder.RenameTable(
                name: "ShoppingLists",
                newName: "ShoppingList");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingList_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
