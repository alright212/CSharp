namespace Domain;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }=default!;
    public string Unit { get; set; }=default!;

    public List<RecipeIngredient>? RecipeIngredients { get; set; }
}