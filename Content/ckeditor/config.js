/// 
/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.languages = 'vi';
  

    config.baseHref = '/Content/PluginCK/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images'
    config.filebrowserBrowseUrl = '/Content/PluginCK/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/PluginCK/ckfinder/ckfinder.html?Types=Images';
    config.filebrowserUploadUrl = '/Content/PluginCK/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=File';
    config.filebrowserImageUploadUrl = '/Content/PluginCK/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';

    CKFinder.setupCKEditor(null, '/Content/ckeditor/ckfinder/');

};
CKEDITOR.on('dialogDefinition', function (ev) {

    var dialogName = ev.data.name,
        dialogDefinition = ev.data.definition;

    if (dialogName == 'image') {
        var onOk = dialogDefinition.onOk;
        dialogDefinition.onOk = function (e) {
            var width = this.getContentElement('info', 'txtWidth');
            var height = this.getContentElement('info', 'txtHeight');
            var currWidth = width.getValue();
            var currHeight = height.getValue();
            if (currWidth > 850) {
                var newWidth = '100%';
                var newHeight = 'auto';
                width.setValue(newWidth);
                height.setValue(newHeight);
            }
            else {
                width.setValue(currWidth);
                height.setValue(currHeight);
            }
            onOk && onOk.apply(this, e);
        };
    }      
});
