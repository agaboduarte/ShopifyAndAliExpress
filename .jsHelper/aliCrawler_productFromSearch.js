/*
    NOTE
        - Use google chrome
        - Paste in cosole: example page: http://www.aliexpress.com/wholesale?catId=0&initiative_id=SB_20160417073605&SearchText=bracelets
        - Get result and paste in your database
*/

(function () {
    var newscript = document.createElement('script');
    newscript.type = 'text/javascript';
    newscript.async = true;
    newscript.src = 'https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js';
    (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(newscript);
})();

setTimeout(function () {
    var link = $.map($('.picRind'), function (v, i) {
        return {
            v: $(v),
            productUrl: $(v).attr('href'),
            storeUrl: $(v).parents('.item').find('.info-more a.store').attr('href')
        }
    });
    var model = $.map(link, function (v, i) {
        var productId = /\d*.html/.exec(v.productUrl);
        var storeId = /store\/\d*/.exec(v.storeUrl);

        try {
            var aj = storeId[0];
        } catch (e) {
            return null;
        }

        return {
            storeId: storeId[0].replace(/\D/gi, ''),
            productId: productId[0].replace(/\D/gi, '')
        }
    });

    var command = '';
    var commandTemplate = 'EXECUTE [dbo].[CreateProductLink] {productId}, {storeId};';

    for (var i = 0; i < model.length; i++) {
        var item = model[i];

        if (!item) continue;

        command += commandTemplate.replace('{storeId}', item.storeId).replace('{productId}', item.productId);
    }

    console.log(command);
}, 1000);