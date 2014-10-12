$('.plupload-control').each(function () {

    var $container = $(this);

    var uploader = new plupload.Uploader({
        browse_button: $container.find('.plupload-select')[0],
        url: $container.data('plupload-url'),
        flash_swf_url: '/Scripts/Moxie.swf',
        silverlight_xap_url: '/Scripts/Moxie.xap'
    });

    uploader.init();

    uploader.bind('FilesAdded', function (up, files) {

        var $filelist = $container.find('.plupload-filelist');
        var html = $filelist.html();

        plupload.each(files, function (file) {
            html += '<div id="' + file.id + '" title="' + file.name + '" class="plupload-file">' + file.name + '</div>';
        });

        $filelist.html(html);
        up.start();
    });

});
