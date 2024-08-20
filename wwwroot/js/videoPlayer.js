window.initializeHlsPlayer = (videoElementId, url) => {
    const video = document.getElementById(videoElementId);
    if (Hls.isSupported()) {
        const hls = new Hls();
        hls.loadSource(url);
        hls.attachMedia(video);
        hls.on(Hls.Events.MANIFEST_PARSED, function () {
            video.play();
        });
    } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = url;
        video.addEventListener('canplay', function () {
            video.play();
        });
    }
};