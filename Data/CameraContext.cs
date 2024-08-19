using Microsoft.EntityFrameworkCore;

namespace SimpleVMSBlazor.Data;

public class CameraContext: DbContext
{
    public CameraContext(DbContextOptions<CameraContext> options): base(options) { }

    public DbSet<Camera> Cameras => Set<Camera>();
}