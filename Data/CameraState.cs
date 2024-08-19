public class CameraState
{
    public event Action<int>? StopRecording;
    public event Action<int>? StartRecording;

    private Dictionary<int, bool> _recordingStatus = [];
    private Dictionary<int, string> _recordingLogs = [];

    public bool GetRecordingStatus(int cameraId)
    {
        return _recordingStatus.ContainsKey(cameraId) && _recordingStatus[cameraId];
    }

    public void SetRecordingStatus(int cameraId, bool value)
    {
        _recordingStatus[cameraId] = value;
        if (value) StartRecording?.Invoke(cameraId);
        else StopRecording?.Invoke(cameraId);
    }

    public string GetRecordingLogs(int cameraId)
        => _recordingLogs[cameraId];

    public void SetRecordingLogs(int cameraId, string value)
    {
        _recordingLogs[cameraId] = value;
    }
}