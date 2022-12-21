;(function () {
    function initBinking () {
        // Choose strategy:     
        // Api strategy examaple settings:
        binking.setDefaultOptions({
            brandLogoPolicy: "auto",
            strategy: 'api',
            apiKey: '2dc08ea72f2756faffd66e0b12a9df99', // Replace it with your API key
            sandbox: true
        });

        binking.setValidationErrors({
            CARD_NUMBER_REQUIRED: "Укажите номер вашей банковской карты",
            CARD_NUMBER_INVALID: "Номер карты содержит недопустимые символы",
            CARD_NUMBER_INCOMPLETE: "Номер карты заполнен не до конца",
            CARD_NUMBER_OVERCOMPLETE: "В номере карты слишком много символов",
            CARD_NUMBER_LUHN: "В номере карты содержится опечатка",
            MONTH_REQUIRED: "Укажите месяц истечения карты",
            MONTH_INVALID: "Ошибка в месяце истечения карты",
            YEAR_REQUIRED: "Укажите год истечения карты",
            YEAR_INVALID: "Ошибка в годе истечения карты",
            YEAR_IN_PAST: "Этот год прошёл",
            MONTH_IN_PAST: "Этот месяц прошёл",
            CODE_REQUIRED: "Укажите код безопасности",
            CODE_INVALID: "Код безопасности указан неверно"
        });
    }

    function initValidationTips () {
        tippy.setDefaultProps({
            trigger: 'manual',
            hideOnClick: false,
            theme: 'custom',
            distance: 8
        });
        cardNumberTip = tippy($cardNumberField);
        monthTip = tippy($monthField);
        yearTip = tippy($yearField);
        codeTip = tippy($codeField);
    }

    function initMasks () {
        cardNumberMask = IMask($cardNumberField, {
            mask: binking.defaultResult.cardNumberMask
        });
        monthMask = IMask($monthField, {
            mask: IMask.MaskedRange,
            from: 1,
            to: 12,
            maxLength: 2,
            autofix: true
        });
        yearMask = IMask($yearField, {
            mask: '00'
        });
        codeMask = IMask($codeField, {
            mask: '0000'
        });
    }

    function validate () {
        var validationResult = binking.validate($cardNumberField.value, $monthField.value, $yearField.value, $codeField.value);
        if (validationResult.errors.cardNumber && cardNumberTouched) {
            cardNumberTip.setContent(validationResult.errors.cardNumber.message);
            cardNumberTip.show();
        } else {
            cardNumberTip.hide();
        }
        var monthHasError = validationResult.errors.month && monthTouched;
        if (monthHasError) {
            monthTip.setContent(validationResult.errors.month.message);
            monthTip.show();
        } else {
            monthTip.hide();
        }
        if (!monthHasError && validationResult.errors.year && yearTouched) {
            yearTip.setContent(validationResult.errors.year.message);
            yearTip.show();
        } else {
            yearTip.hide();
        }
        if (validationResult.errors.code && codeTouched) {
            codeTip.setContent(validationResult.errors.code.message);
            codeTip.show();
        } else {
            codeTip.hide();
        }
        return validationResult;
    }

    function deselectCards () {
        selectedCardIndex = null;
        var $cards = document.querySelectorAll('.binking__card');
        $cards.forEach(function ($card) {
            $card.classList.remove('binking__selected');
        })
    }

    function resetForm () {
        deselectCards();
        $error.classList.add('binking__hide');
        cardNumberTouched = false;
        monthTouched = false;
        yearTouched = false;
        codeTouched = false;
        cardNumberMask.unmaskedValue = '';
        monthMask.unmaskedValue = '';
        yearMask.unmaskedValue = '';
        codeMask.unmaskedValue = '';
        $saveCardField.checked = false;
        $form.classList.remove('binking__hide');
        $success.classList.add('binking__hide');
    }

    function cardNumberChangeHandler () {
        binking($cardNumberField.value, function (result) {
            newCardInfo = result;
            cardNumberMask.updateOptions({
                mask: result.cardNumberMask
            });
            if (result.formBackgroundColor === '#eeeeee'){
                $frontPanel.style.background = '#1f1c2e';
            }
            else{
                $frontPanel.style.background = result.formBackgroundColor;
            }
            //$frontPanel.style.color = result.formTextColor;
            $frontFields.forEach(function (field) {
                field.style.borderColor = result.formBorderColor;
            })
            $codeField.placeholder = result.codeName || '';
            if (result.formBankLogoBigSvg) {
                $bankLogo.src = result.formBankLogoBigSvg;
                $bankLogo.classList.remove('binking__hide');
            } else {
                $bankLogo.classList.add('binking__hide');
            }
            if (result.formBrandLogoSvg) {
                $brandLogo.src = result.formBrandLogoSvg;
                $brandLogo.classList.remove('binking__hide');
            } else {
                $brandLogo.classList.add('binking__hide');
            }
            var validationResult = validate();
            var isFulfilled = result.cardNumberNormalized.length >= result.cardNumberMinLength;
            var isChanged = prevNumberValue !== $cardNumberField.value;
            if (isChanged && isFulfilled) {
                if (validationResult.errors.cardNumber) {
                    cardNumberTouched = true;
                    validate();
                } else {
                    $monthField.focus();
                }
            }
            prevNumberValue = $cardNumberField.value;
        })
    }

    function cardNumberFocusHandler () {
        deselectCards();
    }

    function cardNumberBlurHandler () {
        cardNumberTouched = true;
        validate();
    }

    function monthChangeHandler () {
        var validationResult = validate();
        if (prevMonthValue !== $monthField.value && $monthField.value.length >= 2) {
            if (validationResult.errors.month) {
                monthTouched = true;
                validate();
            } else {
                $yearField.focus();
            }
        }
        prevMonthValue = $monthField.value;
    }

    function monthFocusHandler () {
        deselectCards();
    }

    function monthBlurHandler () {
        if ($monthField.value.length === 1) {
            monthMask.unmaskedValue = '0' + $monthField.value;
        }
        monthTouched = true;
        validate();
    }

    function yearChangeHandler () {
        var validationResult = validate();
        if (prevYearValue !== $yearField.value && $yearField.value.length >= 2) {
            if (validationResult.errors.year) {
                yearTouched = true;
                validate();
            } else {
                $codeField.focus();
            }
        }
        prevYearValue = $yearField.value;
    }

    function yearFocusHandler () {
        deselectCards();
    }

    function yearBlurHandler () {
        yearTouched = true;
        validate();
    }

    function codeChangeHandler () {
        validate();
    }

    function codeFocusHandler () {
        deselectCards();
    }

    function codeBlurHandler () {
        codeTouched = true;
        validate();
    }
    
    function formSubmitHandler (e) {
        e.preventDefault();
        cardNumberTouched = true;
        monthTouched = true;
        yearTouched = true;
        codeTouched = true;
        var validationResult = validate();
        if (validationResult.hasErrors) {
            $error.classList.remove('binking__hide');
            $error.innerHTML = 'проверьте данные карты';
            return;
        }
        
        $submitButton.style.display = 'none';
        $successIcon.innerHTML = '<?xml version="1.0" encoding="utf-8"?>\n' +
            '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="margin: auto; display: block; shape-rendering: auto;" width="100px" height="100px" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid">\n' +
            '  <g transform="rotate(0 50 50)">\n' +
            '    <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '      <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.9166666666666666s" repeatCount="indefinite"></animate>\n' +
            '    </rect>\n' +
            '  </g><g transform="rotate(30 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.8333333333333334s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(60 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.75s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(90 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.6666666666666666s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(120 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.5833333333333334s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(150 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.5s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(180 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.4166666666666667s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(210 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.3333333333333333s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(240 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.25s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(270 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.16666666666666666s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(300 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="-0.08333333333333333s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g><g transform="rotate(330 50 50)">\n' +
            '  <rect x="47" y="24" rx="3" ry="6" width="6" height="12" fill="currentColor">\n' +
            '    <animate attributeName="opacity" values="1;0" keyTimes="0;1" dur="1s" begin="0s" repeatCount="indefinite"></animate>\n' +
            '  </rect>\n' +
            '</g>\n' +
            '</svg>'
        setTimeout(()=> {
            $successIcon.innerHTML = '';
            $.ajax({
                type: "POST",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                url:`/SubscribePayment/Payment`,
                data:{
                    CardViewModel:{
                        CardNumber: $cardNumberField.value.trim(),
                        CCID: $codeField.value,
                        ExpirationMonth: $monthField.value,
                        ExpirationYear: $yearField.value
                    },
                    SubscribeId: document.querySelector('#subscribeId').value
                },
                success: function (result){
                    onSuccessPayment(result);
                },
                error: function () {
                    console.log('error occured');
                }
            });
        },1000)
    }
    
    function onSuccessPayment(result){
        if(result.succeeded){
            $error.classList.add('binking__hide');
            $success.classList.remove('binking__hide');
            $submitButton.remove();
            $successIcon.classList.remove('binking__hide');
            $successIcon.innerHTML = '        <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="green" class="bi bi-check2-circle" viewBox="0 0 16 16">\n' +
                '                        <path d="M2.5 8a5.5 5.5 0 0 1 8.25-4.764.5.5 0 0 0 .5-.866A6.5 6.5 0 1 0 14.5 8a.5.5 0 0 0-1 0 5.5 5.5 0 1 1-11 0z"/>\n' +
                '                         <path d="M15.354 3.354a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0l7-7z"/>\n' +
                '                        </svg>';
            let timeOutCounter = 11;
            setInterval(()=>{
                timeOutCounter -= 1;
                if(timeOutCounter === 0){
                    window.location.href = '/';
                }
                if(timeOutCounter < 0){
                    timeOutCounter = 0;
                }
                $success.innerHTML = `<h2 class="binking__title">Оплата произведена успешно</h2><br><span>автоматическая переадресация на главную страницу через ${timeOutCounter}</span>`
            },1000);
        }
        else{
            $submitButton.style.display = 'block'
            console.log('fail');
            $success.classList.add('binking__hide');
            $error.classList.remove('binking__hide');
            $error.innerHTML = result.error;
        }
    }
    
    function resetButtonClickHandler () {
        resetForm();
    }

    var $successIcon = document.querySelector('.binking__success-payment'); 
    var $form = document.querySelector('.binking__form');
    var $success = document.querySelector('.binking__success');
    var $submitButton = document.querySelector('.binking__submit-button');
    var $resetButton = document.querySelector('.binking__reset-button');
    var $frontPanel = document.querySelector('.binking__front-panel');
    var $bankLogo = document.querySelector('.binking__form-bank-logo');
    var $brandLogo = document.querySelector('.binking__form-brand-logo');
    var $cardNumberField = document.querySelector('.binking__number-field');
    var $codeField = document.querySelector('.binking__code-field');
    var $monthField = document.querySelector('.binking__month-field');
    var $yearField = document.querySelector('.binking__year-field');
    var $saveCardField = document.querySelector('.binking__save-card-checkbox');
    var $frontFields = document.querySelectorAll('.binking__front-fields .binking__field');
    var $error = document.querySelector('.binking__error');
    var prevNumberValue = $cardNumberField.value;
    var prevMonthValue = $monthField.value;
    var prevYearValue = $yearField.value;
    var selectedCardIndex = null;
    var cardNumberTouched = false;
    var monthTouched = false;
    var yearTouched = false;
    var codeTouched = false;
    var newCardInfo;
    var cardNumberTip;
    var monthTip;
    var yearTip;
    var codeTip;
    var cardNumberMask;
    var monthMask;
    var yearMask;
    var codeMask;
    initBinking();
    initMasks();
    initValidationTips();
    $cardNumberField.addEventListener('input', cardNumberChangeHandler);
    $cardNumberField.addEventListener('focus', cardNumberFocusHandler);
    $cardNumberField.addEventListener('blur', cardNumberBlurHandler);
    $monthField.addEventListener('input', monthChangeHandler);
    $monthField.addEventListener('focus', monthFocusHandler);
    $monthField.addEventListener('blur', monthBlurHandler);
    $yearField.addEventListener('input', yearChangeHandler);
    $yearField.addEventListener('focus', yearFocusHandler);
    $yearField.addEventListener('blur', yearBlurHandler);
    $codeField.addEventListener('input', codeChangeHandler);
    $codeField.addEventListener('focus', codeFocusHandler);
    $codeField.addEventListener('blur', codeBlurHandler);
    $form.addEventListener('submit', formSubmitHandler);
    $resetButton.addEventListener('click', resetButtonClickHandler);
})();