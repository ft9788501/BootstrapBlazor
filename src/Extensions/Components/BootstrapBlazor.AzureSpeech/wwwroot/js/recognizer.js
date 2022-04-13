import { speech_recognizeOnce, close } from "./microsoft-cognitiveservices-speech-sdk-bundle.js"

export function bb_speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage) {
    speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage)
}

export function bb_close(obj, method) {
    close(obj, method)
}
