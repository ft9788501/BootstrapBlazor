let audio = undefined;

export function bb_baidu_speech_synthesizerOnce(obj, callback, data) {
    audio = document.createElement("audio");
    audio.controls = true;
    audio.style.display = "none";
    document.body.appendChild(audio);

    var blob = new Blob([data], { type: 'audio/mp3' });
    var url = (window.URL || webkitURL).createObjectURL(blob);
    audio.src = url;
    audio.addEventListener("ended", function () {
        document.body.removeChild(audio);
        audio = undefined;
        obj.invokeMethodAsync(callback, "Finished");
    });
    audio.play();
}

export function bb_baidu_close_synthesizer(obj, callback) {
    if (audio != undefined) {
        audio.pause();
        document.body.removeChild(audio);
    }
    obj.invokeMethodAsync(callback, "Finished");
}
