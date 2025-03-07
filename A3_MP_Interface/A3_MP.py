
# WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

# This file was generated from A3_MP.idl
# using RTI Code Generator (rtiddsgen) version 4.3.0.
# The rtiddsgen tool is part of the RTI Connext DDS distribution.
# For more information, type 'rtiddsgen -help' at a command shell
# or consult the Code Generator User's Manual.

from dataclasses import field
from typing import Union, Sequence, Optional
import rti.idl as idl
from enum import IntEnum
import sys
import os


@idl.struct
class A3MPDataMsg:
    hour: idl.int16 = 0
    minute: idl.int16 = 0
    second: idl.int16 = 0
    boundingBoxXCoord: float = 0.0
    boundingBoxYCoord: float = 0.0
    boundingBoxLengthPixels: float = 0.0
    boundingBoxWidthPixels: float = 0.0
    isAligned: bool = False
