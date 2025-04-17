
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


@idl.struct(
    member_annotations = {
        'data': [idl.bound(255)],
    }
)
class A3MPDataMsg:
    data: str = "hour=16\nminute=17\nsecond=2\nboundingBoxX=32\nboundingBoxY=32\nboundingBoxWidth=5\nboundingBoxLength=5"
