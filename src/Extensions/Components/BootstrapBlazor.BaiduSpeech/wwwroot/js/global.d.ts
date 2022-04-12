declare class Recorder {
    public constructor(options: Options)
    public open(success: () => void, error: (msg: String, isUserNotAllow: Boolean) => void): void;
    public start();
    public stop(success: (blob: Blob, duration: Number) => void, eror: (msg: String) => void): void;

}

declare interface Obj {
    invokeMethodAsync(method: String, params?: any)
}

interface Options {
    type: String,
    sampleRate: Number,
    bitRate: Number,
    onProcess: (buffers: any, powerLevel: any, bufferDuration: any, bufferSampleRate: any, newBufferIdx: any, asyncEnd: any) => void
}
