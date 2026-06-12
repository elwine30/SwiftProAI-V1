var abp = abp || {};
(function () {
    var showMessage = function (type, message, title, options) {
        options = options || {};
        options.titleText = title;
        options.icon = type;
        options.confirmButtonText = options.confirmButtonText || abp.localization.localize('Ok', 'ThinknInsurTech');

        if (options.isHtml) {
            options.html = htmlMessage;
        } else {
            if(message.includes('\n')){
                const messageLines = message.split('\n').filter(line => line.trim() !== '');
                const htmlMessage = `
                    <ul style="text-align: left; list-style-type: none; padding-left: 20px;">
                        ${messageLines.map(line => `<li style="padding-bottom: 5px;">${line}</li>`).join('')}
                    </ul>
                `;
                options.html = htmlMessage;
            } else{
                options.text = message;
            }
        }

        const { isHtml, ...optionsSafe } = options;
        return Swal.fire(optionsSafe);
    };

    abp.message.info = function (message, title, options) {
        return showMessage('info', message, title, options);
    };

    abp.message.success = function (message, title, options) {
        return showMessage('success', message, title, options);
    };

    abp.message.warn = function (message, title, options) {
        return showMessage('warning', message, title, options);
    };

    abp.message.error = function (message, title, options) {
        console.log("error")
        return showMessage('error', message, title, options);
    };

    abp.message.confirm = function (message, title, callback, options) {
        options = options || {};
        options.title = title ? title : abp.localization.localize('AreYouSure', 'ThinknInsurTech');
        options.icon = options.icon || 'warning';

        options.confirmButtonText = options.confirmButtonText || abp.localization.localize('Yes', 'ThinknInsurTech');
        options.cancelButtonText = options.cancelButtonText || abp.localization.localize('Cancel', 'ThinknInsurTech');
        options.showCancelButton = options.showCancelButton === false ? false : true;
        options.reverseButtons = true;

        if (options.isHtml) {
            options.html = message;
        } else {
            options.text = message;
        }
        const { isHtml, ...optionsSafe } = options;
        return Swal.fire(optionsSafe).then(function(result) {
            callback && callback(result.value, result);
        });
    };
})();
