let rec: Recorder;
let isStart: boolean;
let handler: number

export function bb_baidu_speech_recognizeOnce(obj: Obj, beginRecognize: string, recognizeCallback: string) {
    isStart = true;
    rec = new Recorder({ type: "wav", sampleRate: 16000, bitRate: 16 });

    rec.open(function () {
        rec.start();

        // 通知 UI 开始接收语音
        obj.invokeMethodAsync(beginRecognize, "bb_start");

        handler = setTimeout(function () {
            bb_baidu_speech_close(obj, "bb_timeout", recognizeCallback);
        }, 5000);
    }, function (msg, isUserNotAllow) {
        console.log((isUserNotAllow ? "UserNotAllow，" : "") + "无法录音:" + msg);
    });
}

export function bb_baidu_speech_close(obj: Obj, recognizerStatus: string, recognizeCallback: string) {
    console.log("close");
    if (handler != 0) {
        clearTimeout(handler);
        handler = 0;
    }
    if (isStart) {
        isStart = false;
        rec.stop((blob, duration) => {
            var reader = blob.stream().getReader();
            reader.read().then(value => {
                obj.invokeMethodAsync(recognizeCallback, "bb_finish", value.value);
            })
        }, msg => {
            obj.invokeMethodAsync(recognizeCallback, "bb_error", null);
        });
    }
}
