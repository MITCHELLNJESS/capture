
# WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

# This file was generated from StringSupport.idl
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


StringSupport = idl.get_module("StringSupport")

@idl.struct(
    type_annotations = [idl.type_name("StringSupport::StringMsg")],
    member_annotations = {
        'data': [idl.bound(255)],
    }
)
class StringSupport_StringMsg:
    data: str = ""

StringSupport.StringMsg = StringSupport_StringMsg
