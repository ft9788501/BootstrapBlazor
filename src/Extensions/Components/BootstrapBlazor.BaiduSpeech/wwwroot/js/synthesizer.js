let audio = undefined;
export function bb_baidu_speech_synthesizerOnce(obj, callback, data) {
    var blob = new Blob([data], { type: 'audio/mp3' });
    audio = document.createElement("audio");
    audio.controls = true;
    audio.style.display = "none";
    document.body.appendChild(audio);
    var url = (window.URL || webkitURL).createObjectURL(blob);
    audio.src = url;
    audio.play();
    audio.addEventListener("ended", function () {
        document.body.removeChild(audio);
        audio = undefined;
        obj.invokeMethodAsync(callback, "Finished");
    });
}
export function bb_baidu_close_synthesizer(obj, callback) {
    if (audio != undefined) {
        audio.pause();
        document.body.removeChild(audio);
    }
    obj.invokeMethodAsync(callback, "Finished");
}
