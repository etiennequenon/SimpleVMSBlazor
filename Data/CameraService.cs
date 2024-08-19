using Microsoft.EntityFrameworkCore;
namespace SimpleVMSBlazor.Data;

public class CameraService
{
    private readonly CameraContext _context;

    public CameraService(CameraContext context)
    {
        _context = context;
    }

    public List<Camera> GetCameras()
    {
        return _context.Cameras.ToList();
    }

    public Camera GetCamera(int cameraId)
    {
        return _context.Cameras.Find(cameraId);
    }

    public async Task<List<Camera>> GetCamerasAsync()
    {
        return await _context.Cameras.ToListAsync();
    }

    public async Task<Camera?> GetCameraByIdAsync(int id)
    {
        return await _context.Cameras.FindAsync(id);
    }

    public async Task AddCameraAsync (Camera camera)
    {
        _context.Cameras.Add(camera);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCameraAsync (Camera camera)
    {
        _context.Entry(camera).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCameraAsync(int id)
    {
        var camera = await _context.Cameras.FindAsync(id);
        if (camera != null)
        {
            _context.Cameras.Remove(camera);
            await _context.SaveChangesAsync();
        }
    }
}