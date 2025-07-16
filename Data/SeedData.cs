using Shopping_Cart.Models;
using Microsoft.EntityFrameworkCore;

namespace Shopping_Cart.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Aplica migraciones pendientes (opcional)
            context.Database.Migrate();

            // Si ya hay productos, no insertar de nuevo
            if (context.Products.Any())
            {
                return;
            }

            var products = new List<Product>
            {
                new Product { Name = "Laptop Dell XPS 15", Price = 1500m, Description = "Laptop potente para trabajo y gaming.", ImageUrl = "/images/laptop1.jpg" },
                new Product { Name = "Mouse Logitech MX Master", Price = 80m, Description = "Mouse inalámbrico avanzado.", ImageUrl = "/images/mouse1.jpg" },
                new Product { Name = "Teclado mecánico Razer", Price = 120m, Description = "Teclado para gamers con iluminación RGB.", ImageUrl = "/images/keyboard1.jpg" },
                new Product { Name = "Monitor 27\" 4K", Price = 400m, Description = "Monitor ultra HD con gran detalle.", ImageUrl = "/images/monitor1.jpg" },
                new Product { Name = "Auriculares Bose QC35", Price = 300m, Description = "Auriculares con cancelación de ruido.", ImageUrl = "/images/headphones1.jpg" },
                new Product { Name = "Tablet Samsung Galaxy Tab", Price = 350m, Description = "Tablet ligera y potente.", ImageUrl = "/images/tablet1.jpg" },
                new Product { Name = "Smartphone iPhone 14", Price = 999m, Description = "Último modelo de iPhone.", ImageUrl = "/images/phone1.jpg" },
                new Product { Name = "Disco duro externo 2TB", Price = 100m, Description = "Almacenamiento portátil y rápido.", ImageUrl = "/images/hdd1.jpg" },
                new Product { Name = "Cámara Canon EOS", Price = 650m, Description = "Cámara réflex digital profesional.", ImageUrl = "/images/camera1.jpg" },
                new Product { Name = "Altavoces JBL", Price = 150m, Description = "Altavoces bluetooth potentes.", ImageUrl = "/images/speaker1.jpg" },
                new Product { Name = "Router WiFi 6", Price = 130m, Description = "Router con alta velocidad y cobertura.", ImageUrl = "/images/router1.jpg" },
                new Product { Name = "Smartwatch Apple Watch", Price = 400m, Description = "Reloj inteligente con múltiples funciones.", ImageUrl = "/images/watch1.jpg" },
                new Product { Name = "Silla ergonómica", Price = 200m, Description = "Silla cómoda para oficina.", ImageUrl = "/images/chair1.jpg" },
                new Product { Name = "Impresora HP", Price = 180m, Description = "Impresora multifunción.", ImageUrl = "/images/printer1.jpg" },
                new Product { Name = "Micrófono USB", Price = 90m, Description = "Micrófono para streaming y podcast.", ImageUrl = "/images/microphone1.jpg" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
