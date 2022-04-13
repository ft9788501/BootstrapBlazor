import { SpeechTranslationConfig, AudioConfig, TranslationRecognizer } from 'microsoft-cognitiveservices-speech-sdk'

var recognizer = null;

export function bb_close(obj, method) {
    if (recognizer != null) {
        recognizer.close();
    }
    obj.invokeMethodAsync(method, '');
}

export function bb_speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage) {

    var speechConfig = SpeechTranslationConfig.fromAuthorizationToken(token, region);
    speechConfig.speechRecognitionLanguage = recognitionLanguage;
    speechConfig.addTargetLanguage(targetLanguage)

    var audioConfig = AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new TranslationRecognizer(speechConfig, audioConfig);

    recognizer.recognizeOnceAsync(function (successfulResult) {
        recognizer.close();
        recognizer = null;
        obj.invokeMethodAsync(method, successfulResult.privText);
    }, function (err) {
        console.log(err);
    });
}







