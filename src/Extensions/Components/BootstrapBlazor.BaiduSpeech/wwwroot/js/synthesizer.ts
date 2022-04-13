export function bb_speech_synthesizerOnce(obj: Obj, callback: String, data: Uint8Array) {
    var blob = new Blob([data], { type: 'audio/mp3' });
    console.log(blob);
    var audio = document.createElement("audio");
    audio.controls = true;
    audio.style.display = "none";
    document.body.appendChild(audio);
    var url = (window.URL || webkitURL).createObjectURL(blob);
    audio.src = url;
    audio.play();
}
