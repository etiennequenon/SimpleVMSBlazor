using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SimpleVMSBlazor.Data;
using System.Data.SqlTypes;

namespace SimpleVMSBlazor.Services;

public class VideoRecordingService : IHostedService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<VideoRecordingService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CameraState _cameraState;
    private readonly string? _recordingProcessBinLocation;
    private readonly string? _recordingRootDirectory;
    private readonly Dictionary<int, IRecorder> _recorders = [];

    public VideoRecordingService(IConfiguration configuration, 
                                 ILogger<VideoRecordingService> logger, 
                                 IServiceProvider serviceProvider, 
                                 CameraState cameraState)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _cameraState = cameraState;
        _cameraState.StartRecording += StartRecordingEvent;
        _cameraState.StopRecording += StopRecordingEvent;
        _recordingProcessBinLocation = _configuration["RecordingService:RecordingProcessBinLocation"];
        _recordingRootDirectory = _configuration["RecordingService:RecordingDirectory"];
    }

    private IRecorder CreateRecorder(string ressourceUrl, string recordingDirectory)
    {
        return new FFmpegRecorder(ressourceUrl, recordingDirectory, _recordingProcessBinLocation);
    }

    private string CreateRecordingFolder(string ressourceName)
    {
        string newDirectory = $"{_recordingRootDirectory}/{ressourceName}";
        if(!Directory.Exists(newDirectory))
            Directory.CreateDirectory(newDirectory);
        return newDirectory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting video recording service...");
        using (var scope = _serviceProvider.CreateScope())
        {
            var cameraService = scope.ServiceProvider.GetRequiredService<CameraService>();
            List<Camera> cameras = cameraService.GetCameras();
            foreach (Camera camera in cameras)
            {
                if(camera.Recording)
                {
                    string recordingDirectory = CreateRecordingFolder(camera.Name);
                    _recorders[camera.Id] = CreateRecorder($"rtsp://{camera.Username}:{camera.Password}@{camera.Address}/axis-media/media.amp", recordingDirectory);
                    _recorders[camera.Id].StartRecording();
                    _cameraState.SetRecordingStatus(camera.Id, true);
                }
            }
        }
        _logger.LogInformation($"Recording started. Saving to {_recordingRootDirectory}");
        return Task.CompletedTask;
    }

    public void StartRecordingEvent(int cameraId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var cameraService = scope.ServiceProvider.GetRequiredService<CameraService>();
            Camera camera = cameraService.GetCamera(cameraId);
            if(!_recorders.ContainsKey(camera.Id))
            {
                string recordingDirectory = CreateRecordingFolder(camera.Name);
                _recorders[camera.Id] = CreateRecorder($"rtsp://{camera.Username}:{camera.Password}@{camera.Address}/axis-media/media.amp", recordingDirectory);
                _recorders[camera.Id].StartRecording();
                _cameraState.SetRecordingStatus(camera.Id, true);
            }
            _logger.LogInformation($"Recording started for {camera.Address}#{camera.Id}");
        }
    }

    public void StopRecordingEvent(int cameraId)
    {
        if(_recorders.ContainsKey(cameraId))
            _recorders[cameraId].StopRecording();
        _logger.LogInformation($"Recording stopped for CameraId: {cameraId}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping video recording service...");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        return;
    }
}
