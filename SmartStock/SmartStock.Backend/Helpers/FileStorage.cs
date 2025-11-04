using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SmartStock.Backend.Helpers;

public class FileStorage : IFileStorage
{
    //private readonly string _connectionString;

    //public FileStorage(IConfiguration configuration)
    //{
    //    _connectionString = configuration.GetConnectionString("AzureStorage")!;
    //}

    //public async Task RemoveFileAsync(string path, string containerName)
    //{
    //    var client = new BlobContainerClient(_connectionString, containerName);
    //    await client.CreateIfNotExistsAsync();
    //    var fileName = Path.GetFileName(path);
    //    var blob = client.GetBlobClient(fileName);
    //    await blob.DeleteIfExistsAsync();
    //}

    //public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
    //{
    //    var client = new BlobContainerClient(_connectionString, containerName);
    //    await client.CreateIfNotExistsAsync();
    //    client.SetAccessPolicy(PublicAccessType.Blob);
    //    var fileName = $"{Guid.NewGuid()}{extention}";
    //    var blob = client.GetBlobClient(fileName);

    //    using (var ms = new MemoryStream(content))
    //    {
    //        await blob.UploadAsync(ms);
    //    }

    //    return blob.Uri.ToString();
    //}

    private readonly IWebHostEnvironment _env;

    public FileStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
    {
        // 🔧 Si el contenedor no incluye "images", lo agrega automáticamente
        if (!containerName.Contains("images"))
            containerName = Path.Combine("images", containerName);

        // 🧭 Ruta física completa dentro de wwwroot
        var folder = Path.Combine(_env.WebRootPath, containerName);

        // 🗂️ Crear la carpeta si no existe
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        // 📝 Generar nombre único
        var fileName = $"{Guid.NewGuid()}{(extension.StartsWith(".") ? extension : "." + extension)}";
        var filePath = Path.Combine(folder, fileName);

        // 💾 Guardar el archivo localmente
        await File.WriteAllBytesAsync(filePath, content);

        // 🌐 Retornar ruta accesible desde el navegador
        var relativePath = Path.Combine("/", containerName, fileName).Replace("\\", "/");
        return relativePath;
    }

    public Task RemoveFileAsync(string path, string containerName)
    {
        if (string.IsNullOrEmpty(path)) return Task.CompletedTask;

        // 🔧 Normalizar contenedor
        if (!containerName.Contains("images"))
            containerName = Path.Combine("images", containerName);

        var fileName = Path.GetFileName(path);
        var fullPath = Path.Combine(_env.WebRootPath, containerName, fileName);

        // 🗑️ Eliminar si existe
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}