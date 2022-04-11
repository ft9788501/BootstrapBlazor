
let rec: Recorder;

export function bb_speech_init(obj: Obj, callback: String, method: String) {
    rec = new Recorder({
        type: "wav", sampleRate: 16000, bitRate: 16, onProcess: (buffers, powerLevel, bufferDuration, bufferSampleRate, newBufferIdx, asyncEnd) => {
            var buffer = new Uint16Array([0x45, 0x76, 0x65, 0x72, 0x79, 0x74, 0x68, 0x69,
                0x6e, 0x67, 0x27, 0x73, 0x20, 0x73, 0x68, 0x69, 0x6e, 0x79, 0x2c,
                0x20, 0x43, 0x61, 0x70, 0x74, 0x69, 0x61, 0x6e, 0x2e, 0x20, 0x4e,
                0x6f, 0x74, 0x20, 0x74, 0x6f, 0x20, 0x66, 0x72, 0x65, 0x74, 0x2e]);
            //obj.invokeMethodAsync(method, buffer);
        }
    });

    rec.open(function () {
    }, function (msg, isUserNotAllow) {
        console.log((isUserNotAllow ? "UserNotAllow，" : "") + "无法录音:" + msg);
    });
}

export function bb_speech_start() {
    rec.start();
}

export function bb_speech_close(obj: Obj, callback: String, method: String, translation: String) {
    rec.stop((blob, duration) => {
        var reader = blob.stream().getReader();
        reader.read().then(value => {
            obj.invokeMethodAsync(translation, value.value);
            console.log();
        })
    }, (msg) => { });
}

interface Obj {
    invokeMethodAsync(method: String, parms?: any)
}
