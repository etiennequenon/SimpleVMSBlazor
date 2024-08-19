using System.Diagnostics;

namespace SimpleVMSBlazor.Services;

public sealed class FFmpegRecorder : IRecorder
{
    private readonly string _ressourceUrl;
    private readonly string _recordingLocation;
    private readonly string _ffmpegLocation;
    private readonly Process _recordingProcess = new Process();

    public FFmpegRecorder(string ressourceUrl, string recordingLocation, string ffmpegLocation)
    {
        _ressourceUrl = ressourceUrl;
        _recordingLocation = recordingLocation;
        _ffmpegLocation = ffmpegLocation;
    }

    public void StartRecording()
    {
        string arguments = $"-i {_ressourceUrl} -c:v libx264 -preset veryfast -maxrate 3000k -bufsize 6000k -vf \"scale=-2:720\" -g 50 -c:a aac -ar 44100 -b:a 128k " +
                           $"-hls_time 4 -hls_list_size 900 -hls_flags delete_segments " +
                           $"-hls_segment_filename \"{_recordingLocation}/segment_%03d.ts\" " +
                           $"-hls_flags append_list+discont_start+program_date_time " +
                           $"-hls_allow_cache 1 -start_number 0 \"{_recordingLocation}/stream.m3u8\"";
        _recordingProcess.StartInfo.FileName = _ffmpegLocation;
        _recordingProcess.StartInfo.Arguments = arguments;
        _recordingProcess.StartInfo.RedirectStandardOutput = true;
        _recordingProcess.StartInfo.RedirectStandardError = true;
        _recordingProcess.StartInfo.UseShellExecute = false;
        _recordingProcess.StartInfo.CreateNoWindow = true;

        _recordingProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        _recordingProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

        _recordingProcess.Start();
        _recordingProcess.BeginErrorReadLine();
        _recordingProcess.BeginOutputReadLine();
    }

    public void StopRecording()
    {
        _recordingProcess.Kill();
        Console.WriteLine("Stop");
    }
}