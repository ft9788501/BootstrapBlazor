import { speech_synthesizerOnce, tts_close } from "./microsoft-cognitiveservices-speech-sdk-bundle.js"
import { speech_recognizeOnce } from "./speech.js"

export function bb_speech_synthesizerOnce(obj, method, token, region, synthesizerLanguage, voiceName, inputText) {
    speech_recognizeOnce(obj, method, token, region, synthesizerLanguage, voiceName, inputText)
}

export function bb_tts_close(obj, method) {
    tts_close(obj, method);
}


