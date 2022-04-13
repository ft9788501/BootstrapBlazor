let rec: Recorder;
let isStart: Boolean;

export function bb_baidu_speech_recognizeOnce(obj: Obj, beginRecognize: String, recognizeCallback: String) {
    isStart = true;
    rec = new Recorder({ type: "wav", sampleRate: 16000, bitRate: 16 });

    rec.open(function () {
        rec.start();

        // 通知 UI 开始接收语音
        obj.invokeMethodAsync(beginRecognize, "bb_start");

        var handler = setTimeout(function () {
            clearTimeout(handler);
            bb_baidu_speech_close(obj, recognizeCallback);
        }, 5000);
    }, function (msg, isUserNotAllow) {
        console.log((isUserNotAllow ? "UserNotAllow，" : "") + "无法录音:" + msg);
    });
}

export function bb_baidu_speech_close(obj: Obj, recognizeCallback: String) {
    console.log("close");
    if (isStart) {
        isStart = false;
        rec.stop((blob, duration) => {
            var reader = blob.stream().getReader();
            reader.read().then(value => {
                obj.invokeMethodAsync(recognizeCallback, value.value);
            })
        }, msg => {
        });
    }
}
