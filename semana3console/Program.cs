using System;
using System.Data.SqlClient;

namespace CRUDProductosConsola
{
    class Program
    {
        static string connectionString = "Server=OCTI\\SQLEXPRESS;Database=Comercio;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("📦 CRUD DE PRODUCTOS");
                Console.WriteLine("1 - Agregar producto");
                Console.WriteLine("2 - Listar productos");
                Console.WriteLine("3 - Modificar producto");
                Console.WriteLine("4 - Eliminar producto");
                Console.WriteLine("0 - Salir");
                Console.Write("Seleccione una opción: ");
                int.TryParse(Console.ReadLine(), out opcion);

                if (opcion == 1) AgregarProducto();
                else if (opcion == 2) ListarProductos();
                else if (opcion == 3) ModificarProducto();
                else if (opcion == 4) EliminarProducto();
                else if (opcion != 0) Console.WriteLine("❌ Opción no válida");

                if (opcion != 0)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 0);
        }

        static void AgregarProducto()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine();

            Console.Write("Precio: ");
            decimal precio = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Stock: ");
            int stock = Convert.ToInt32(Console.ReadLine());

            Console.Write("ID de Categoría: ");
            int categoriaId = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId) VALUES (@nombre, @descripcion, @precio, @stock, @categoriaId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@categoriaId", categoriaId);
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Producto agregado con éxito.");
            }
        }

        static void ListarProductos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT P.Id, P.Nombre, P.Precio, P.Stock, C.Nombre AS Categoria FROM Productos P JOIN Categorias C ON P.CategoriaId = C.Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("📋 Productos:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]} | {reader["Nombre"]} | ${reader["Precio"]} | Stock: {reader["Stock"]} | Categoría: {reader["Categoria"]}");
                }
                reader.Close();
            }
        }

        static void ModificarProducto()
        {
            Console.Write("Ingrese el ID del producto a modificar: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nuevo precio: ");
            decimal nuevoPrecio = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Nueva descripción: ");
            string nuevaDescripcion = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Productos SET Precio = @precio, Descripcion = @descripcion WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@precio", nuevoPrecio);
                cmd.Parameters.AddWithValue("@descripcion", nuevaDescripcion);
                cmd.Parameters.AddWithValue("@id", id);
                int filas = cmd.ExecuteNonQuery();

                Console.WriteLine(filas > 0 ? "🔄 Producto actualizado." : "⚠️ No se encontró el producto.");
            }
        }

        static void EliminarProducto()
        {
            Console.Write("Ingrese el ID del producto a eliminar: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Productos WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                int filas = cmd.ExecuteNonQuery();

                Console.WriteLine(filas > 0 ? "❌ Producto eliminado." : "⚠️ No se encontró el producto.");
            }
        }
    }
}

