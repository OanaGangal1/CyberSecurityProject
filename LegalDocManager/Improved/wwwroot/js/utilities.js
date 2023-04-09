"use strict"

const utilities = function ()
{
    const formDataToJson = function (form)
    {
        let json = {};

        if (form)
        {
            let formAsArray = form.serializeArray();

            if (formAsArray)
            {
                for (let i = 0; i < formAsArray.length; i++)
                {
                    json[formAsArray[i].name] = formAsArray[i].value
                }
            }
        }

        return json;
    }

    const blockUISettings = function ()
    {
        return {
            message: '<div class="bg-secundary"><div class="spinner-border m-2" role="status"></div><div class="m-2">' + "Please wait" + '</div></div>',
            css: {
                border: 'none',
                opacity: 0.5
            }
        };

    }

    const blockUI = function (jquery)
    {
        if (!jquery) {
            $.blockUI(blockUISettings());
            return;
        }

        jquery.block(blockUISettings());
    }

    const unBlockUI = function (jquery)
    {
        if (!jquery) {
            $.unblockUI();
            return;
        }

        jquery.unblock();
    }

    return {
        formDataAsJson: formDataToJson,
        blockUI: blockUI,
        unblockUI: unBlockUI
    };
}();