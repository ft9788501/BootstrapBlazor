let rec;
export function bb_baidu_speech_recognizeOnce(obj, callback, translation) {
    rec = new Recorder({ type: "wav", sampleRate: 16000, bitRate: 16 });
    rec.open(function () {
        rec.start();
        var timer = setTimeout(function () {
            clearTimeout(timer);
            bb_baidu_speech_close(obj, callback, translation);
        }, 5000);
    }, function (msg, isUserNotAllow) {
        console.log((isUserNotAllow ? "UserNotAllow，" : "") + "无法录音:" + msg);
    });
}
export function bb_baidu_speech_close(obj, callback, translation) {
    rec.stop((blob, duration) => {
        var reader = blob.stream().getReader();
        reader.read().then(value => {
            obj.invokeMethodAsync(translation, value.value);
        });
    }, (msg) => { });
}
//# sourceMappingURL=recognizer.js.map