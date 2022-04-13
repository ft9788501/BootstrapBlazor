export function bb_baidu_speech_synthesizerOnce(obj: Obj, callback: String, data: Uint8Array) {
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
