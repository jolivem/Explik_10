/**
 * @fileoverview Implements entry point
 * @author NHN Ent. FE Development Lab <dl_javascript@nhnent.com>
 */

'use strict';

var toMark = require('./to-mark');
var Renderer = require('./renderer');
var basicRenderer = require('./renderer.basic');
var gfmRenderer = require('./renderer.gfm');

toMark.Renderer = Renderer;
toMark.basicRenderer = basicRenderer;
toMark.gfmRenderer = gfmRenderer;

module.exports = toMark;
