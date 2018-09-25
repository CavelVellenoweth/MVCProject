namespace MVCProject.Migrations
{
    using MVCProject.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    enum Categories
    {
        Cat=1,Dog,Reptile,Fish,Mammalian
    };
    internal sealed class Configuration : DbMigrationsConfiguration<MVCProject.Context.MVCPetsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MVCProject.Context.MVCPetsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Categories.AddOrUpdate(x => x.CategoryId,
               new Category() { CategoryName = "Cats", Description = "Cat like things" },
               new Category() { CategoryName = "Dogs", Description = "Dog like things" },
               new Category() { CategoryName = "Reptiles", Description = "Scaley things" },
               new Category() { CategoryName = "Fish", Description = "Fishy things" },
               new Category() { CategoryName = "Mammalians", Description = "Not cats and dogs" }
               );
            context.Products.AddOrUpdate(x => x.ProductID,
                new Product() { ProductName = "Catarina", Description = "Catarina is a gentle soul that likes long naps and the finest of food.", ImagePath = "", Price = 50.99M, CategoryID = (int)Categories.Cat, DiscountPct = 0 },
                new Product() { ProductName = "Dogbert", Description = "Dogbert is one of the goodest boys there is, take him while you can.", ImagePath = "", Price = 69.69M, CategoryID = (int)Categories.Dog, DiscountPct = 0 },
                new Product() { ProductName = "Cat-thrin Zeta Jones", Description = "Cat-thrin is a diva that likes the finer things in life.", ImagePath = "", Price = 99.99M, CategoryID = (int)Categories.Cat, DiscountPct = 0 },
                new Product() { ProductName = "Catt Damon", Description = "Catt Damon is a very lazy boy.", ImagePath = "", Price = 59.99M, CategoryID = (int)Categories.Cat, DiscountPct = 0 },
                new Product() { ProductName = "Tom the Cat", Description = "Tom the Cat is a TomCat.", ImagePath = "", Price = 40.99M, CategoryID = (int)Categories.Cat, DiscountPct = 0 },
                new Product() { ProductName = "Doggy Brown", Description = "Doggy Brown loves music and dancing.", ImagePath = "", Price = 96.96M, CategoryID = (int)Categories.Dog, DiscountPct = 0 },
                new Product() { ProductName = "Dog", Description = "This is Dog, he is Dog.", ImagePath = "", Price = 96.69M, CategoryID = (int)Categories.Dog, DiscountPct = 0 },
                new Product() { ProductName = "Woofy Goldberg", Description = "Woofy is a very passionate doog.", ImagePath = "", Price = 69.96M, CategoryID = (int)Categories.Dog, DiscountPct = 0 },
                new Product() { ProductName = "Snake", Description = "Please we bought this snake by accident and we need to get rid of him we used to stock mice and other small rodents but now because of this snake they're all gone and we don't know what to do please buy this snake and save our business.", ImagePath = "", Price = 1.99M, CategoryID = (int)Categories.Reptile, DiscountPct = 0 },
                new Product() { ProductName = "Fish", Description = "I don't know they're just fish dude just buy them please.", ImagePath = "", Price = 10.99M, CategoryID = (int)Categories.Fish, DiscountPct = 0 },
                new Product() { ProductName = "Rabbit", Description = "Please get this before the snake does.", ImagePath = "", Price = 20.99M, CategoryID = (int)Categories.Mammalian, DiscountPct = 0 },
                new Product() { ProductName = "Hamson The final Hamster", Description = "Armed with the bones of his fallen comrades he alone stays behind to fight of the tyranical snake that has slain his brethren.", ImagePath = "", Price = 1000000M, CategoryID = (int)Categories.Mammalian, DiscountPct = 0 }
                );
        }
    }
}
