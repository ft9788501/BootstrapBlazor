import { SpeechTranslationConfig, AudioConfig, TranslationRecognizer, SpeechSynthesizer, ResultReason } from 'microsoft-cognitiveservices-speech-sdk'

var recognizer = undefined;
var synthesizer = undefined;

export function speech_recognizeOnce(obj, method, token, region, recognitionLanguage, targetLanguage) {

    var speechConfig = SpeechTranslationConfig.fromAuthorizationToken(token, region);
    speechConfig.speechRecognitionLanguage = recognitionLanguage;
    speechConfig.addTargetLanguage(targetLanguage)

    var audioConfig = AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new TranslationRecognizer(speechConfig, audioConfig);

    recognizer.recognizeOnceAsync(function (successfulResult) {
        recognizer.close();
        recognizer = undefined;
        obj.invokeMethodAsync(method, successfulResult.privText);
    }, function (err) {
        console.log(err);
    });
}

export function close_recognizer(obj, method) {
    if (recognizer != undefined) {
        recognizer.close();
    }
    obj.invokeMethodAsync(method, '');
}

export function speech_synthesizerOnce(obj, method, token, region, synthesizerLanguage, voiceName, inputText) {
    var speechConfig = SpeechTranslationConfig.fromAuthorizationToken(token, region);
    speechConfig.speechSynthesisLanguage = synthesizerLanguage;
    speechConfig.speechSynthesisVoiceName = voiceName;
    var audioConfig = AudioConfig.fromDefaultSpeakerOutput();
    synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
    synthesizer.speakTextAsync(
        inputText,
        function (result) {
            if (result.reason === ResultReason.SynthesizingAudioCompleted) {
                console.log("synthesis finished for [" + inputText + "]");
            } else if (result.reason === SpeechSDK.ResultReason.Canceled) {
                console.log("synthesis failed. Error detail: " + result.errorDetails);
            }
            console.log(result);
            synthesizer.close();
            synthesizer = undefined;
            obj.invokeMethodAsync(method, "Finished");
        },
        function (err) {
            console.log(err);

            synthesizer.close();
            synthesizer = undefined;
            obj.invokeMethodAsync(method, "Error");
        });
}

export function close_synthesizer(obj, method) {
    if (synthesizer != undefined) {
        synthesizer.close();
        synthesizer = undefined;
        obj.invokeMethodAsync(method, "Close");
    }
}
