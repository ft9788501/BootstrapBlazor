export function bb_baidu_speech_synthesizerOnce(obj, callback, data) {
    var blob = new Blob([data], { type: 'audio/mp3' });
    var audio = document.createElement("audio");
    audio.controls = true;
    audio.style.display = "none";
    document.body.appendChild(audio);
    var url = (window.URL || webkitURL).createObjectURL(blob);
    audio.src = url;
    audio.play();
    obj.invokeMethodAsync(callback, "Finished");
}
//# sourceMappingURL=synthesizer.js.map