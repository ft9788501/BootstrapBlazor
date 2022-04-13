import { speech_recognizeOnce, close_recognizer } from "./microsoft-cognitiveservices-speech-sdk-bundle.js"

export function bb_speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage) {
    speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage)
}

export function bb_close_recognizer(obj, method) {
    close_recognizer(obj, method)
}
