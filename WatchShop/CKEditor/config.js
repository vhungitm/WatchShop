/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.filebrowserBrowseUrl = '/CKFinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/CKFinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/CKFinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/CKFinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/CKFinder/core/connector/php/connector.php?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/CKFinder/core/connector/php/connector.php?command=QuickUpload&type=Flash';
};
