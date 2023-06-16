window.setPlaybackRate = function (rate) {
    var audios = document.getElementsByTagName("audio");
    for (var i = 0; i < audios.length; i++) {
        audios[i].playbackRate = rate;
    }
}