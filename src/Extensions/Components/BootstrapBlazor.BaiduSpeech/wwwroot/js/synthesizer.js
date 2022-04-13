export function bb_baidu_speech_synthesizerOnce(obj, callback, data) {
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
//# sourceMappingURL=synthesizer.js.map