import { speech_synthesizerOnce, close_synthesizer } from "./microsoft-cognitiveservices-speech-sdk-bundle.js"

export function bb_azure_speech_synthesizerOnce(obj, method, token, region, synthesizerLanguage, voiceName, inputText) {
    console.log(inputText);
    speech_recognizeOnce(obj, method, token, region, synthesizerLanguage, voiceName, inputText);
}

export function bb_azure_close_synthesizer(obj, method) {
    close_synthesizer(obj, method);
}
